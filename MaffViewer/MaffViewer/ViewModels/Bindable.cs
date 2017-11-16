namespace MaffViewer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides Binding Notifications for ViewModels.
    /// </summary>
    public abstract class Bindable : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Notifies subscribers of the property change.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">The property expression.</param>
        protected void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            OnPropertyChanged(property.GetMemberInfo().Name);
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        #endregion

        #region Property Bindings

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>T.</returns>
        protected T GetProperty<T>([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            if (_properties.TryGetValue(propertyName, out var property))
                return (T)property;

            return default(T);
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="changedCallback">The changed callback.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected void SetProperty<T>(T value, Action changedCallback, [CallerMemberName] string propertyName = null)
        {
            if (changedCallback == null)
                throw new ArgumentNullException(nameof(changedCallback));

            UpdateProperty(value, null, propertyName);
            changedCallback();
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="changedCallback">The changed callback.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected void SetProperty<T>(T value, Action<T> changedCallback, [CallerMemberName] string propertyName = null)
        {
            if (changedCallback == null)
                throw new ArgumentNullException(nameof(changedCallback));

            UpdateProperty(value, changedCallback, propertyName);
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected void SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            UpdateProperty(value, null, propertyName);
        }

        /// <summary>
        /// Updates the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="changedCallback">The changed callback.</param>
        /// <param name="propertyName">Name of the property.</param>
        private void UpdateProperty<T>(T value, Action<T> changedCallback, string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            var oldValue = GetProperty<T>(propertyName);
            if (Equals(oldValue, value))
                return;

            _properties[propertyName] = value;
            OnPropertyChanged(propertyName);

            changedCallback?.Invoke(oldValue);
        }

        #endregion
    }

    /// <summary>
    /// ViewModel extension methods
    /// </summary>
    public static class ViewModelExtensionMethods
    {
        /// <summary>
        /// Get's the name of the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The assembly's name.</returns>
        public static string GetAssemblyName(this Assembly assembly)
        {
            return assembly.FullName.Remove(assembly.FullName.IndexOf(','));
        }

        /// <summary>
        /// Converts an expression into a <see cref="MemberInfo"/>.
        /// </summary>
        /// <param name="expression">The expression to convert.</param>
        /// <returns>The member info.</returns>
        public static MemberInfo GetMemberInfo(this Expression expression)
        {
            var lambda = (LambdaExpression)expression;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression unaryExpression)
                memberExpression = (MemberExpression)unaryExpression.Operand;
            else
                memberExpression = (MemberExpression)lambda.Body;

            return memberExpression.Member;
        }
    }
}
