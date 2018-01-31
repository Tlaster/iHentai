using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHentai.Basic
{
    public class NewTabArgs
    {
        public const string NewTab = nameof(NewTab);

        public NewTabArgs(Type viewModel, params object[] @params)
        {
            Params = @params;
            ViewModel = viewModel;
        }

        public object[] Params { get; }
        public Type ViewModel { get; }
    }
}
