namespace MaffViewer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Provides common functionality for ViewModels.
    /// </summary>
    public abstract class ViewModelBase : Bindable
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether this ViewModel is in design mode.
        /// </summary>
        /// <value><c>true</c> if this ViewModel is in design mode; otherwise, <c>false</c>.</value>
        public static bool IsDesignMode => (bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue);

        #endregion
    }
}
