﻿using System;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CosmosDbSampleApp
{
    public class PersonListViewModel : BaseViewModel
    {
        #region Fields
        bool _isDeletingPerson, _isRefreshing;
        IList<PersonModel> _personList;
        ICommand _pullToRefreshCommand;
        #endregion

        #region Events
        public event EventHandler<string> Error;
        #endregion

        #region Properties
        public ICommand PullToRefreshCommand => _pullToRefreshCommand ??
            (_pullToRefreshCommand = new Command(async () => await ExecutePullToRefreshCommand()));

        public IList<PersonModel> PersonList
        {
            get => _personList;
            set => SetProperty(ref _personList, value);
        }

        public bool IsDeletingPerson
        {
            get => _isDeletingPerson;
            set => SetProperty(ref _isDeletingPerson, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        #endregion

        #region Methods
        Task ExecutePullToRefreshCommand() => UpdatePersonList();

        async Task UpdatePersonList()
        {
            try
            {
                IsRefreshing = IsInternetConnectionActive = true;
                PersonList = await DocumentDbService.GetAllPersonModels();
            }
            catch (Exception e)
            {
                OnError(e.InnerException.Message);
                DebugHelpers.PrintException(e);
            }
            finally
            {
                IsRefreshing = IsInternetConnectionActive = false;
            }
        }

        void OnError(string message) => Error?.Invoke(this, message);
        #endregion
    }
}
