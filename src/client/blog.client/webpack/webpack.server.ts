// npm install --save-dev @types/node
// 这里需要装这个包
import * as path from 'path';
const projectRoot = path.resolve(__dirname, '..');//根路径

//定义模块
module.exports = {
  // 此处告知 server bundle 使用 Node 风格导出模块(Node-style exports)
  // 这里必须是node，因为打包完成的运行环境是node，在node端运行的，不是在浏览器端运行。
  target: 'node',
  // entry需要提供一个单独的入口文件
  entry: ['babel-polyfill', path.join(projectRoot, 'entry/entry-server.js')],
  // 输出
  output: {
    //指定libraryTarget的类型为commonjs2，用来指定代码export出去的入口的形式。
    // 在node.js中模块是module.exports = {...}，commonjs2打包出来的代码出口形式就类似于此。
    libraryTarget: 'commonjs2',
    path: path.join(projectRoot, 'dist'), // 打包出的路径
    filename: 'bundle.server.js',// 打包最终的文件名，这个文件是给 node 服务器使用的
  },
  module: {
    // 因为使用webpack2，这里必须是rules，如果使用use，
    // 会报个错：vue this._init is not a function
    rules: [
      //规则1、vue规则定义
      {
        test: /\.vue$/,
        loader: 'vue-loader',
      },//js规则定义
      {
        test: /\.js$/,
        loader: 'babel-loader',
        include: projectRoot,
        // 这里会把node_modules里面的东西排除在外，提高打包效率
        exclude: /node_modules/,
        // ES6 语法
        options: {
          presets: ['es2015']
        }
      },//css定义
      {
        test: /\.less$/,
        loader: "style-loader!css-loader!less-loader"
      }
    ]
  },
  plugins: [],
  resolve: {
    alias: {
      'vue$': 'vue/dist/vue.runtime.esm.js'
    }
  }
}