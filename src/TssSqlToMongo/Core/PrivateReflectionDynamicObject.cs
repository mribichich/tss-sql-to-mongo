namespace TssSqlToMongo.Core
{
    using System;
    using System.Dynamic;
    using System.Reflection;

    internal class PrivateReflectionDynamicObject : DynamicObject
    {
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;

        public object RealObject { get; set; }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = InvokeMemberOnType(this.RealObject.GetType(), this.RealObject, binder.Name, args);

            // Wrap the sub object if necessary. This allows nested anonymous objects to work.
            result = WrapObjectIfNeeded(result);

            return true;
        }

        internal static object WrapObjectIfNeeded(object o)
        {
            // Don't wrap primitive types, which don't have many interesting internal APIs
            if (o == null || o.GetType()
                                 .IsPrimitive || o is string)
            {
                return o;
            }

            return new PrivateReflectionDynamicObject { RealObject = o };
        }

        private static object InvokeMemberOnType(Type type, object target, string name, object[] args)
        {
            var argtypes = new Type[args.Length];

            for (var i = 0; i < args.Length; i++)
            {
                argtypes[i] = args[i].GetType();
            }

            while (true)
            {
                var member = type.GetMethod(name, BindingFlags, null, argtypes, null);

                if (member != null)
                {
                    return member.Invoke(target, args);
                }

                if (type.BaseType == null)
                {
                    return null;
                }

                type = type.BaseType;
            }
        }
    }
}