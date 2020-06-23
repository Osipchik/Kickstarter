const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {
    entry: {
        app: ['babel-polyfill', path.resolve(__dirname, 'src/index.js')],
        silent_renew: [path.resolve(__dirname, 'silent_renew/index.js')]
    },
    output: {
        path: path.join(__dirname, '/dist'),
        pathinfo: true,
        filename: '[name].bundle.js',
        chunkFilename: '[name].chunk.js',
        publicPath: '/'
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: ['babel-loader']
            },
            {
                test: /\.css$/,
                use: ['style-loader', 'css-loader'],
            },
            {
                test: /\.html$/,
                use: ["html-loader"]
            },
            {
                test: /\.(png|svg|jpg|gif|webp)$/,
                use: ['file-loader']
            },
        ]
    },
    devServer: {
        http2: true,
        https: true,
        compress: true,
        historyApiFallback: true,
        port: 3000,
        hot: true,
        open: true
    },
    plugins: [
        new HtmlWebpackPlugin({
            template: './src/index.html',
            favicon: './src/assets/favicon.ico',
            chunks: ['app'],
            filename: 'index.html'
        }),
        new HtmlWebpackPlugin({
            template: './silent_renew/silent_renew.html',
            chunks: ['silent_renew'],
            filename: 'silent_renew.html'
        }),

    ]
}