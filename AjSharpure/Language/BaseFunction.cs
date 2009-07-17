namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    public abstract class BaseFunction : BaseObject, IFunction, ISerializable
    {
        public BaseFunction()
        {
        }

        public BaseFunction(IPersistentMap metadata)
            : base(metadata)
        {
        }

        public override IObject WithMetadata(IPersistentMap metadata)
        {
            throw new NotSupportedException();
        }

        public object Call(Machine machine, ValueEnvironment environment)
        {
            return this.Apply(machine, environment, null);
        }

        #region IFunction Members

        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            throw new NotImplementedException();
        }

        public bool IsMacro
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
