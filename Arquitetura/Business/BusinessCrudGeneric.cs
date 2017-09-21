using Arquitetura.Erro;
using Arquitetura.Helper;
using Arquitetura.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Arquitetura.Business
{
    public class BusinessCrudGeneric<TContext, TRepository, TEntity, TDTO>
        where TContext : DbContext, new()
        where TRepository : IGenericRepository<TContext, TEntity>, new()
        where TEntity : class
        where TDTO : class
    {
        public readonly TRepository repository;

        public BusinessCrudGeneric()
        {
            if (repository == null)
                repository = new TRepository();
        }

        public BusinessCrudGeneric(TContext context)
        {
            repository = new TRepository();
            repository.Contexto = context;
        }

        public TRepository Repository
        {
            get { return repository; }
        }

        protected TContext Contexto
        {
            get { return Repository.Contexto; }
        }

        public TEntity Incluir(TDTO dto, bool salvarAlteracoes = true)
        {
            ValidarCamposObrigatorios(dto, ECrudOperacao.Criar);

            ValidarRegrasDeNegocio(dto, ECrudOperacao.Criar);

            TEntity resultado = null;
            if (typeof(TDTO) == typeof(TEntity))
                TransactionHelper.PersistirEmTransacao(this.Contexto, delegate () { resultado = IncluirModel(dto as TEntity, salvarAlteracoes); });
            else
                TransactionHelper.PersistirEmTransacao(this.Contexto, delegate () { resultado = IncluirDTO(dto, salvarAlteracoes); });

            return resultado;
        }

        protected virtual TEntity IncluirModel(TEntity model, bool salvarAlteracoes = true)
        {
            repository.Create(model, salvarAlteracoes);
            return model;
        }

        protected virtual TEntity IncluirDTO(TDTO dto, bool salvarAlteracoes = true)
        {
            throw new NotImplementedException();
        }

        public virtual void ValidarCamposObrigatorios(TDTO dto, ECrudOperacao crudAction)
        {
            var excecoes = ValidarCamposObrigatoriosAnotados(dto, crudAction, false);
            ValidarCamposObrigatorios(dto, excecoes, crudAction);
            if (excecoes.Count > 0)
                throw new ExcecaoCampoObrigatorio(excecoes);
        }

        protected virtual void ValidarCamposObrigatorios(TDTO dto, IList<KeyValuePair<string, string>> excecoesAnotadas, ECrudOperacao crudAction)
        {
        }

        protected virtual void ValidarRegrasDeNegocio(TDTO dto, ECrudOperacao crudAction)
        {
        }


        private object ObterValorDaPropriedade(PropertyInfo propriedade, object objetoMetaData, object objetoEntiy)
        {
            if (objetoMetaData != null)
            {
                // parametro property veio de um objeto MetaData, obter property do objeto Entity
                PropertyInfo propEntity = objetoEntiy.GetType().GetProperty(propriedade.Name);
                return propEntity.GetValue(objetoEntiy, null);
            }

            // parametro property veio de um objeto Entity
            return propriedade.GetValue(objetoEntiy, null);
        }

        private PropertyInfo[] ObterPropriedadesDoModel(object modelComAnotacoesRequired, ref object objetoMetaData)
        {
            MetadataTypeAttribute metaDataAttr = (MetadataTypeAttribute)modelComAnotacoesRequired.GetType().GetCustomAttributes(false).FirstOrDefault(a => a is MetadataTypeAttribute);
            if (metaDataAttr == null) // o model não possui uma classe MetaData associada, usar metadata do EDMX ou model
                return modelComAnotacoesRequired.GetType().GetProperties();

            // obtem propriedades da classe MetaData
            objetoMetaData = Activator.CreateInstance(metaDataAttr.MetadataClassType);
            return objetoMetaData.GetType().GetProperties();
        }

        public IList<KeyValuePair<string, string>> ValidarCamposObrigatoriosAnotados(object modelComAnotacoesRequired, ECrudOperacao crudAction, bool throwException = true)
        {
            IList<KeyValuePair<string, string>> excecoes = new List<KeyValuePair<string, string>>();
            object objetoMetadados = null;
            PropertyInfo[] propriedades;
            bool campoVazio;

            if (modelComAnotacoesRequired == null)
                return excecoes;

            propriedades = ObterPropriedadesDoModel(modelComAnotacoesRequired, ref objetoMetadados);

            foreach (PropertyInfo prop in propriedades)
            {
                RequiredAttribute requiredAttribute = (RequiredAttribute)prop.GetCustomAttributes(false).SingleOrDefault(attr => attr is RequiredAttribute);

                bool requerido = (requiredAttribute != null);

                if (requerido)
                {
                    campoVazio = false;

                    // verifica se o valor da propriedade é vazio
                    object valorDaPropriedade = ObterValorDaPropriedade(prop, objetoMetadados, modelComAnotacoesRequired);

                    Type type = prop.PropertyType;
                    if (type.Name.ToLower() == "string")
                    {
                        campoVazio = string.IsNullOrWhiteSpace((valorDaPropriedade as string));
                    }
                    else if (type.Name.ToLower().StartsWith("int"))
                    {
                        campoVazio = Convert.ToInt64(valorDaPropriedade) <= 0;
                    }
                    else if (type.Name == "Byte") // tipo byte nunca é null // Enum do tipo Byte deve iniciar com valor 1
                    {
                        campoVazio = (Convert.ToByte(valorDaPropriedade) == 0);
                    }
                    else if (type.Name.ToLower() == "datetime")
                    {
                        DateTime data = (DateTime)valorDaPropriedade;
                        campoVazio = (data == null || data == new DateTime() || data.Year < 1800);
                    }
                    else if (type.Name.ToLower() == "decimal")
                    {
                        decimal? valor = (decimal?)valorDaPropriedade;
                        campoVazio = (valor.GetValueOrDefault() <= 0);
                    }
                    else if (type.Name.StartsWith("IList"))
                    {
                        System.Collections.IList lista = (valorDaPropriedade as System.Collections.IList);
                        campoVazio = (lista == null || lista.Count == 0);
                        if (!campoVazio)
                        {
                            // verificar obrigatoriedade das propriedades do objeto da lista
                            for (int i = 0; i < lista.Count; i++)
                            {
                                object itemList = lista[i];
                                IList<KeyValuePair<string, string>> excecoesItemLista = ValidarCamposObrigatoriosAnotados(itemList, crudAction, false);
                                foreach (var item in excecoesItemLista)
                                    InserirExcecao(excecoes, mensagem: item.Value, campoAssociado: item.Key);
                            }
                        }
                    }
                    else if (prop.PropertyType.IsEnum)
                    {
                        campoVazio = !prop.PropertyType.IsEnumDefined(valorDaPropriedade);
                    }
                    else if (new[] { "boolean" }.Contains(type.Name.ToLower()) == false) // tipos q não pode ter valor vazio atribuido
                    {
                        // objetos
                        if (valorDaPropriedade == null)
                            campoVazio = true;
                        else
                        {
                            IList<KeyValuePair<string, string>> excecoesItemLista = ValidarCamposObrigatoriosAnotados(valorDaPropriedade, crudAction, false);
                            foreach (var item in excecoesItemLista)
                                InserirExcecao(excecoes, mensagem: item.Value, campoAssociado: item.Key);
                        }
                    }

                    if (campoVazio)
                    {
                        string errorMsg = null;
                        if (requiredAttribute == null) // validação de EntityObject via atributos do Edmx
                        {
                            errorMsg = prop.Name + ": campo obrigatório.";
                        }
                        else // validação via atributo Required
                        {
                            if (string.IsNullOrWhiteSpace(requiredAttribute.ErrorMessage))
                            {
                                // obtem DisplayName
                                DisplayNameAttribute atributoDisplayName = (DisplayNameAttribute)prop.GetCustomAttributes(false).SingleOrDefault(attr => attr is DisplayNameAttribute);
                                string displayName = atributoDisplayName == null ? prop.Name : atributoDisplayName.DisplayName;

                                errorMsg = (displayName ?? prop.Name) + ": campo obrigatório.";
                            }
                            else
                                errorMsg = requiredAttribute.ErrorMessage;
                        }

                        // trata referencia a outra propriedade do mesmo objeto na mensagem de erro no formato "Informe a data de emissão da NFP #[Numero]."
                        if (errorMsg != null && errorMsg.Contains('[') && errorMsg.Contains(']'))
                        {
                            string[] parts = errorMsg.Split('['); // "Informe a data de emissão da NFP #[Numero]." resulta em [0] = "Informe a data de emissão da NFP #", [1] = "Numero]."
                            if (parts.Length >= 2)
                            {
                                string[] subparts = parts[1].Split(']'); // "Numero]" resulta em [0] = "Numero", [1] = "."
                                if (subparts.Length > 0)
                                {
                                    string referencedProperty = (!subparts[0].Contains('[') ? subparts[0] : ""); // verifica se não há outro [ antes do fechamento (])
                                    if (!string.IsNullOrWhiteSpace(referencedProperty) && propriedades.Any(p => p.Name == referencedProperty))
                                    {
                                        string valueOfReferencedProperty = propriedades.Single(p => p.Name == referencedProperty).GetValue(modelComAnotacoesRequired, null).ToString();
                                        errorMsg = errorMsg.Replace("[" + referencedProperty + "]", valueOfReferencedProperty);
                                    }
                                }
                            }
                        }

                        InserirExcecao(excecoes, errorMsg, campoAssociado: prop.Name);
                    }
                }
            }

            if (excecoes.Count > 0 && throwException)
                throw new ExcecaoCampoObrigatorio(modelComAnotacoesRequired.GetType(), excecoes);

            return excecoes;
        }

        private void InserirExcecao(IList<KeyValuePair<string, string>> excecoes, string mensagem, string campoAssociado)
        {
            if (!excecoes.Any(e => e.Value == mensagem))
                excecoes.Add(campoAssociado, mensagem);
        }

    }

    public class BusinessCrudGeneric<TContext, TEntity> : BusinessCrudGeneric<TContext, GenericRepository<TContext, TEntity>, TEntity, TEntity>
        where TContext : DbContext, new()
        where TEntity : class
    {
        public BusinessCrudGeneric() { }
        public BusinessCrudGeneric(TContext context) : base(context) { }
    }
}
