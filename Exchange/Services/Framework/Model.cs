using System.Collections.Generic;
using System.ComponentModel;

namespace Kadevjo.Core.Models
{
    public abstract class Model : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the model identifier.
        /// </summary>
        /// <value>The model identifier.</value>
        //public abstract object ModelId { get; set; }

        /// <summary>
        /// Gets or sets the id for use in chache.
        /// </summary>
        /// <value>The identifier.</value>

        //protected string id;

        //public abstract string Id { get; set; }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}