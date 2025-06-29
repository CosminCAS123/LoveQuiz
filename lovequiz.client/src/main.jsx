import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './index.css'
import App from './App.jsx'
import GenderBlock from './GenderPage.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path="/"       element={<App />} />
        <Route path="/gender" element={<GenderBlock />} />
      </Routes>
    </BrowserRouter>
  </StrictMode>
)
