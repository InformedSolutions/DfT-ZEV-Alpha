const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
    entry: {
        'gds.site': './js/gds/site.js',
        'gds.back': './js/gds/back.js',
    },
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, '..', 'wwwroot', 'dist')
    },
    devtool: 'source-map',
    mode: 'development',
    module: {
        rules: [
            {
                test: /\.css$/i,
                use: [MiniCssExtractPlugin.loader, "css-loader"],
            }
        ]
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery',
            'window.$': 'jquery',
        }),
        new webpack.ProvidePlugin({
            'window.GOVUKFrontend': './js/gds/govuk-frontend-3.9.1.min.js',
        }),
        new MiniCssExtractPlugin()
    ]
};
