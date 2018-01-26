using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.Mvc.Render
{
    /// <summary>
    /// Render Data Grid script on page.
    /// </summary>
    public class GridScriptRender
    {
        /// <summary>
        /// The data grid id on page.
        /// </summary>
        public string GridId = "grid";
        /// <summary>
        /// Grid list psot url.
        /// </summary>
        public string DataLoadUrl = "";
        /// <summary>
        /// Grid list columns.
        /// { field: 'ck', checkbox: true },
        /// { title: 'ID', field: 'RoleID', width: 30, hidden: true,sorter: sorterFn},
        /// { title: 'Mark', field: 'Mark', width: 100, sortable: true },
        /// { title: 'State', field: 'State', width: 50, sortable: true, formatter: ShowState }
        /// </summary>
        public string Rows = "";
        /// <summary>
        /// Add or edit operation url.
        /// </summary>
        public string AddOrEditOperationUrl = "";
        /// <summary>
        /// Add operation tab title.
        /// </summary>
        public string AddOperationTabTitle = "";
        /// <summary>
        /// Edit operation tab title.
        /// </summary>
        public string EditOperationTabTitle = "";
        /// <summary>
        /// Delete operation url.
        /// </summary>
        public string DeleteOperationUrl = "";
        /// <summary>
        /// Grid default primary key field.
        /// </summary>
        public string IdField = "";
        /// <summary>
        /// Grid default sort field.
        /// </summary>
        public string SortName = "";
        /// <summary>
        /// Grid default sort order.
        /// </summary>
        public string SortOrder = "";
        /// <summary>
        /// Grid user extend property.
        /// ExtendProperty="property1:'value1',property2:'value2',";
        /// </summary>
        public string ExtendProperty = "";
        /// <summary>
        /// Grid override SearchBar.
        /// OverrideSearchBar=@"{
        ///         text: 'Search',
        ///         iconCls: 'icon-search',
        ///         handler: function () {
        ///             //override block.
        ///         }
        ///     }";
        /// </summary>
        public string OverrideSearchBar = "";
        /// <summary>
        /// Grid override SearchBar.
        /// ExtendToolsBar=@"{
        ///         text: 'Title',
        ///         iconCls: 'icon-className',
        ///         handler: function () {
        ///             //code block.
        ///         }
        ///     }";
        /// </summary>
        public string ExtendToolsBar = "";

        /// <summary>
        /// Create Grid Script
        /// </summary>
        /// <param name="addable">Useable add bar.</param>
        /// <param name="editable">Useable edit bar.</param>
        /// <param name="deleteable">Useable delete bar.</param>
        /// <param name="searchable">Useable search bar.</param>
        /// <returns>Script</returns>
        public System.Web.Mvc.MvcHtmlString CreateGridDefaultScript(bool? addable, bool? editable, bool? deleteable, bool? searchable)
        {
            string AddBar = @"{
                    text: 'Add',
                    iconCls: 'icon-add',
                    handler: function () {
                        parent.AddTab(AddOperationTabTitle, AddOrEditOperationUrl);
                    }
                }";
            string EditBar = @"{
                    text: 'Edit',
                    iconCls: 'icon-edit',
                    handler: function () {
                        EditRowFn('" + GridId + @"');
                    }
                }";
            string DeleteBar = @"{
                    text: 'Delete',
                    iconCls: 'icon-remove',
                    handler: function () {
                        DeleteAllFun('" + GridId + @"', DeleteOperationUrl);
                    }
                }";
            string SearchBar = (OverrideSearchBar=="" ? @"{
                    text: 'Search',
                    iconCls: 'icon-search',
                    handler: function () {
                    }
                }" : OverrideSearchBar);

            string gridStr = @"
                <script language='javascript' type='text/javascript'>
                    var DataLoadUrl = '" + DataLoadUrl + @"';
                    " + ((addable == true || editable == true) ? "var AddOrEditOperationUrl = '" + AddOrEditOperationUrl + @"';var AddOperationTabTitle = '" + AddOperationTabTitle + @"';" : "") +
                        (deleteable == true ? "var EditOperationTabTitle = '" + EditOperationTabTitle + @"';" : "") +
                        (searchable == true ? "var DeleteOperationUrl = '" + DeleteOperationUrl + @"';" : "") +
                    @"$(function () {
                        $('#grid').datagrid($.extend(DefaultDataGridProperties, {" +
                            (IdField == "" ? "" : ("idField: '" + IdField + "',")) +
                            (SortName == "" ? "" : ("sortName: '" + SortName + "',")) +
                            (SortOrder == "" ? "" : ("sortOrder: '" + SortOrder + "',")) + ExtendProperty +
                            @"url: DataLoadUrl,
                            queryParams: {},
                            columns: [[" + Rows + @"]],
                            toolbar: [" +
                                        ((addable == true ? AddBar + "," : "") +
                                        (editable == true ? EditBar + "," : "") +
                                        (deleteable == true ? DeleteBar + "," : "") +
                                        (ExtendToolsBar == "" ? "" : ExtendToolsBar + ",") +
                                        (searchable == true ? SearchBar + "," : "")).TrimEnd(',') +
                                    @"]
                        }));
                    });
                </script>";

            return new System.Web.Mvc.MvcHtmlString(gridStr);
        }
    }
}
