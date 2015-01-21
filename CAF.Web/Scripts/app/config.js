
//requirejs.config({
//     baseUrl: '/scripts/app', paths: { app: 'app' }
//});
//requirejs(['app'], function(app) {
//     app.hello();
//});

requirejs.config({
    baseUrl: '/scripts/app',
    paths: { hi: 'hello' },
    shim: {
        hello: {
//hi只是下面引用的一个名称而已
            //               exports: 'hello'
            init: function() {
                return {
                    hello: hello,
                    hello2: hello2
                }
            }
        }
    }
});
//requirejs(['hello'], function(hi) {
//     hello();
//});
requirejs(['hello'], function (o) {
    o.hello();
o.hello2();
});