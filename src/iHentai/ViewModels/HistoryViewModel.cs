using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Data;
using iHentai.Data.Models;

namespace iHentai.ViewModels
{
    class HistoryViewModel : ViewModelBase
    {
        public ObservableCollection<ReadingHistoryModel> Source => ReadingHistoryDb.Instance.Source;
    }
}
