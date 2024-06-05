import {createProxyMiddleware} from 'http-proxy-middleware';

module.exports = function (app) {
  app.use(
    '/api', 
    createProxyMiddleware({
      target: import.meta.env.VITE_API_URL,
      //target: 'http://github:5000', 
      //target: "http://lebedev-systems.de:5000",
      // target: 'http://github',
      //target: 'https://lebedev-systems.de/',
      changeOrigin: true,
    })
  );
};
