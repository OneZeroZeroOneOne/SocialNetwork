const webpack = require('webpack')

module.exports = {
    runtimeCompiler: true,
    configureWebpack: {
        /*plugins: [
            new webpack.ContextReplacementPlugin(/moment[/\\]locale$/, /ru|en/),
        ],*/
        resolve: {
            alias: {
              'vue$': 'vue/dist/vue.common.js' // 'vue/dist/vue.common.js' for webpack 1
            }
        }
    }
}