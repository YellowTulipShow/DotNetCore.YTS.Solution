(function() {
    window.BasicCRUQ = function(args) {
        this.args = args;
        this.search_form = Object.get(args, 'search_form', null);
        this.datagrid = Object.get(args, 'datagrid', null);
        this.tableName = Object.get(args, 'tableName', '数据表');
        this.dialog = Object.get(args, 'dialog', null);
        this.dialog_form = Object.get(args, 'dialog_form', null);
        this.save_data_method = Object.get(args, 'save_data_method', null);
    }
    window.BasicCRUQ.prototype = {
        GetSearchData: function() {
            var self = this;
            return self.search_form ? $(self.search_form).serializeJSON() : {};
        },
        Search: function() {
            var self = this;
            if (self.datagrid) {
                $(self.datagrid).datagrid('load', self.GetSearchData());
            }
            return false;
        },
        Create: function() {
            var self = this;
            $(self.dialog).dialog({
                onLoad: function() {
                    $(this).window('center');
                },
            });
            $(self.dialog).dialog('open').dialog('setTitle', '添加' + self.tableName);
        },
        Edit: function() {
            var self = this;
            var row = $(self.datagrid).datagrid('getSelected');
            if (!row) {
                $.messager.alert("提示", "未选择数据！", "info");
                return;
            }
            $(self.dialog).dialog({
                onLoad: function() {
                    $(this).window('center');
                    $(self.dialog_form).form('load', row);
                },
            });
            $(self.dialog).dialog('open').dialog('setTitle', '编辑' + self.tableName);
        },
        Remove: function() {
            var self = this;
            var removeApiAddress = Object.get(self.args, 'api_remove', null);
            if (!removeApiAddress) {
                $.messager.alert({
                    title: '提示',
                    msg: '未定义删除API地址, 请联系管理员!',
                    icon: 'error',
                });
                return;
            }

            var rows = $(self.datagrid).datagrid('getSelections');
            if (!rows.length) {
                $.messager.alert({
                    title: '提示',
                    msg: '请选择要删除的项。',
                    icon: 'info'
                });
                return;
            }
            $.messager.confirm('提示', '确认删除选中的项？', function(isOk) {
                if (!isOk)
                    return;
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    var id = Object.get(rows[i], 'ID', '');
                    if (id)
                        ids.push(id);
                }
                Api.CallPost({
                    'url': removeApiAddress,
                    'data': {
                        'IDs': ids,
                    },
                    success: function (json) {
                        json = json || {};
                        if (Object.get(json, 'Code', 404) != 200) {
                            $.messager.alert({
                                title: '提示',
                                msg: Object.get(json, 'Message', '错误请求'),
                                icon: 'error'
                            });
                            return;
                        }
                        $(self.dialog).dialog('close');
                        $(self.datagrid).datagrid('reload');
                    },
                    error: function(xhr) { self.errorMethod(xhr) },
                });
            });
        },
        Save: function() {
            var self = this;
            var editApiAddress = Object.get(self.args, 'api_edit', null);
            if (!editApiAddress) {
                $.messager.alert({
                    title: '提示',
                    msg: '未定义保存数据API地址, 请联系管理员!',
                    icon: 'error',
                });
                return;
            }

            if (!$(self.dialog_form).form('validate')) {
                $.messager.alert({
                    title: '提示',
                    msg: '数据校验不通过, 请检查',
                    icon: 'error'
                });
                return false;
            }
            var model = null;
            if (self.save_data_method) {
                model = self.save_data_method();
            } else {
                model = $(self.dialog_form).serializeJSON();
            }
            Api.CallPost({
                'url': editApiAddress,
                'data': {
                    'model': model,
                },
                success: function (json) {
                    json = json || {};
                    if (Object.get(json, 'Code', 404) != 200) {
                        $.messager.alert({
                            title: '提示',
                            msg: Object.get(json, 'Message', '错误请求'),
                            icon: 'error'
                        });
                        return;
                    }
                    $(self.dialog).dialog('close');
                    if (Object.get(model, 'ID', 0) == 0) {
                        $(self.datagrid).datagrid('load');
                    } else {
                        $(self.datagrid).datagrid('reload');
                    }
                },
                error: function(xhr) { self.errorMethod(xhr) },
            });
        },
        errorMethod: function(xhr) {
            var self = this;
            var responseJSON = Object.get(xhr, 'responseJSON', {});
            var responseStatus = Object.get(responseJSON, 'status', 0);
            var responseTitle = Object.get(responseJSON, 'title', '');
            var status = Object.get(xhr, 'status', 0);
            var statusText = Object.get(xhr, 'statusText', '');
            $.messager.alert({
                title: '提示',
                msg: responseTitle || statusText,
                icon: 'error'
            });
        },
    };
})();
