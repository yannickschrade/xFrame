using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xFrame.Core.MVVM;

namespace WPFTestApp
{
    public class ViewModel : ViewModelBase
    {
        [MaxLength(5, ErrorMessage = "Text zu lang")]
        public string Text
        {
            get => Get<string>();
            set => SetAndValidateWithAnnotations(value);
        }
    }
}
