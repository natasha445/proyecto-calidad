using System;

namespace ProyectoCalidad.Validators
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateParamAttribute : Attribute
    {
        public Type ModelType { get; set; }

        public ValidateParamAttribute(Type model)
        {
            ModelType = model;
        }
    }
}
