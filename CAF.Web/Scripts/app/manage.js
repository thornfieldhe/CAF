var User = Backbone.Epoxy.Model.extend({
    defaults: {
        LoginName: '',
        Name: '',
        Pass: '',
        PhoneNum: '',
        Email:''
    }
});

var view=new Backbone.Epoxy.View({
    el:'',
    model:new User()});