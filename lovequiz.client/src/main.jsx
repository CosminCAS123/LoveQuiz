import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './index.css'
import App from './App.jsx'
import GenderBlock from './GenderPage.jsx'
import {FemaleBlock, MaleBlock} from './GenderPage.jsx'
import Results, {PaidResults} from './Results.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path="/"       element={<App />} />
        <Route path="/gender" element={<GenderBlock />} />
        <Route path="/gender/female" element={<FemaleBlock />} />
        <Route path="/gender/male" element={<MaleBlock />} />
        <Route path="/results" element={<Results />} />
        <Route path="/results/paid" element={<PaidResults />} />
      </Routes>
    </BrowserRouter>
  </StrictMode>
)
