import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import { BrowserRouter } from 'react-router-dom'
import { ConfigProvider, theme  } from 'antd'


ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <ConfigProvider theme={{
      token:{
        //colorBgBase: "#515151",
        colorPrimary: "#732dd8",
        colorInfo: "#732dd8"},
        algorithm: theme.defaultAlgorithm
      }} >
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </ConfigProvider>
  </React.StrictMode>,
)
