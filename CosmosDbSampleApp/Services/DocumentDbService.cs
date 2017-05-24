﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Azure.Documents.Client;

namespace CosmosDbSampleApp
{
    public static class DocumentDbService
    {
        #region Constant Fields
        static readonly DocumentClient _readonlyClient = new DocumentClient(new Uri(DocumentDbConstants.Url), DocumentDbConstants.ReadOnlyPrimaryKey);
        static readonly DocumentClient _readWriteClient = new DocumentClient(new Uri(DocumentDbConstants.Url), DocumentDbConstants.ReadWritePrimaryKey);
        static readonly Uri _documentCollectionUri = UriFactory.CreateDocumentCollectionUri(PersonModel.DatabaseId, PersonModel.CollectionId);
        #endregion

        public static async Task<List<PersonModel>> GetAllPersonModels()
        {
            return await Task.Run(() => _readonlyClient.CreateDocumentQuery<PersonModel>(_documentCollectionUri).ToList());
        }

        public static async Task<PersonModel> GetPersonModel(string id)
        {
            var result = await _readonlyClient.ReadDocumentAsync<PersonModel>(
                UriFactory.CreateDocumentUri(PersonModel.DatabaseId, PersonModel.CollectionId, id));

            if (result.StatusCode != System.Net.HttpStatusCode.Created)
                return null;

            return result;
        }

        public static async Task<PersonModel> CreatePersonModel(PersonModel person)
        {
            var result = await _readWriteClient.CreateDocumentAsync(_documentCollectionUri, person);

            if (result.StatusCode != System.Net.HttpStatusCode.Created)
                return null;

            return (PersonModel)result.Resource;
        }
    }
}