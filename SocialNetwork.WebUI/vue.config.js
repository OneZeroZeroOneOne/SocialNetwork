const webpack = require('webpack')

module.exports = {
    runtimeCompiler: true,
    configureWebpack: {
        plugins: [
            new webpack.ContextReplacementPlugin(/moment[/\\]locale$/, /ru|en/),
        ]
    }
}