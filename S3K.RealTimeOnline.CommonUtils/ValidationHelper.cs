using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace S3K.RealTimeOnline.CommonUtils
{
    public class ValidationHelper
    {

        public static bool ValidateProperties<T>(object obj)
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            foreach (PropertyInfo propertyInfo in obj.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                string propertyName = propertyInfo.Name;
                if (!typeof(T).HasProperty(propertyName))
                {
                    throw new ValidationException(string.Format("The '{0}' property is not valid.", propertyName));
                }            
            }

            return true;
        }

        public static bool ValidateProperties<T>(IDictionary<string, object> obj)
            where T : class
        {
            if (obj == null || !obj.Any())
            {
                throw new ArgumentNullException("obj");
            }


            foreach (KeyValuePair<string, object> property in obj)
            {
                string propertyName = property.Key;
                if (!typeof(T).HasProperty(propertyName))
                {
                    throw new ValidationException(string.Format("The '{0}' property is not valid.", propertyName));
                }
            }

            return true;
        }

        public static bool ValidateProperties<T>(ExpandoObject obj)
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            foreach (KeyValuePair<string, object> property in obj)
            {
                string propertyName = property.Key;
                if (!typeof(T).HasProperty(propertyName))
                {
                    throw new ValidationException(string.Format("The '{0}' property is not valid.", propertyName));
                }
            }

            return true;
        }

        public static bool ValidateValues<T>(object obj, out IList<ValidationResult> validationResults)
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            validationResults = new List<ValidationResult>();
            foreach (PropertyInfo propertyInfo in obj.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                string propertyName = propertyInfo.Name;
                if (!typeof(T).HasProperty(propertyName))
                {
                    continue;
                }

                IList<ValidationAttribute> validationAttributes = new List<ValidationAttribute>();

                object[] attributes = propertyInfo.GetCustomAttributes(true);
                foreach (object attribute in attributes)
                {
                    ValidationAttribute validationAttribute = attribute as ValidationAttribute;
                    if (validationAttribute != null)
                    {
                        validationAttributes.Add(validationAttribute);
                    }
                }

                if (validationAttributes.Any())
                {
                    object value = propertyInfo.GetValue(obj, null);
                    Validator.TryValidateValue(value,
                        new ValidationContext(obj, null, null),
                        validationResults,
                        validationAttributes);
                }
            }

            return !validationResults.Any();
        }

        public static void ValidateValues<T>(object obj)
            where T : class
        {
            if (!ValidateValues<T>(obj, out IList<ValidationResult> validationResults))
                throw new ValidationException(validationResults.Select(x => x.ErrorMessage)
                    .Aggregate((i, j) => i + "," + j));
        }

        public static bool ValidateValues<T>(ExpandoObject obj, out IList<ValidationResult> validationResults)
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            validationResults = new List<ValidationResult>();
            foreach (KeyValuePair<string, object> property in obj)
            {
                string propertyName = property.Key;
                object value = property.Value;
                if (!typeof(T).HasProperty(propertyName))
                {
                    continue;
                }

                PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
                if (propertyInfo == null) continue;
                IList<ValidationAttribute> validationAttributes = propertyInfo
                    .GetCustomAttributes(true)
                    .OfType<ValidationAttribute>()
                    .ToArray();


                if (validationAttributes.Any())
                {
                    Validator.TryValidateValue(value, new ValidationContext(value, null, null)
                        {
                            MemberName = propertyName
                        },
                        validationResults, validationAttributes);
                }
            }

            return !validationResults.Any();
        }

        public static void ValidateValues<T>(ExpandoObject obj)
            where T : class
        {
            if (!ValidateValues<T>(obj, out IList<ValidationResult> validationResults))
                throw new ValidationException(validationResults.Select(x => x.ErrorMessage)
                    .Aggregate((i, j) => i + "," + j));
        }

        public static bool ValidateValues<T>(IDictionary<string, object> obj,
            out IList<ValidationResult> validationResults)
            where T : class
        {
            if (obj == null || !obj.Any())
            {
                throw new ArgumentNullException("obj");
            }

            validationResults = new List<ValidationResult>();

            foreach (KeyValuePair<string, object> property in obj)
            {
                string propertyName = property.Key;
                object value = property.Value;

                if (!typeof(T).HasProperty(propertyName))
                {
                    bool isColumnName = false;
                    foreach (PropertyInfo info in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        ColumnAttribute columnAttribute =
                            info.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();
                        if (columnAttribute != null && columnAttribute.Name == propertyName)
                        {
                            propertyName = info.Name;
                            isColumnName = true;
                            break;
                        }
                    }

                    if (!isColumnName)
                    {
                        continue;
                    }
                }

                IList<ValidationAttribute> validationAttributes = new List<ValidationAttribute>();
                PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
                if (propertyInfo == null) continue;
                object[] attributes = propertyInfo.GetCustomAttributes(true);
                foreach (object attribute in attributes)
                {
                    ValidationAttribute validationAttribute = attribute as ValidationAttribute;
                    if (validationAttribute != null)
                    {
                        validationAttributes.Add(validationAttribute);
                    }
                }

                if (validationAttributes.Any())
                {
                    Validator.TryValidateValue(value, new ValidationContext(value, null, null)
                        {
                            MemberName = propertyName
                        },
                        validationResults, validationAttributes);
                }
            }

            return !validationResults.Any();
        }

        public static void ValidateValues<T>(IDictionary<string, object> obj)
            where T : class
        {
            if (!ValidateValues<T>(obj, out IList<ValidationResult> validationResults))
                throw new ValidationException(validationResults.Select(x => x.ErrorMessage)
                    .Aggregate((i, j) => i + "," + j));
        }

        public static bool ValidateObject(object obj, out IList<ValidationResult> validationResults)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj,
                new ValidationContext(obj, null, null),
                validationResults, true);
            return isValid;
        }

        public static void ValidateObject(object obj)
        {
            if (!ValidateObject(obj, out IList<ValidationResult> validationResults))
                throw new ValidationException(validationResults.Select(x => x.ErrorMessage)
                    .Aggregate((i, j) => i + "," + j));
        }
    }
}