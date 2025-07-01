import React from 'react';
import './App.css';
import Header from './Header.jsx'
import MainBlock from './Landing_page.jsx';
import Footer from './Footer.jsx';
import './tailwind.css';
import FloatingHearts from './FloatingHeats.jsx';

function App() {
    const handleClick = () => {
        alert('Salut! Ai apasat butonul 🚀');
    };

    return (
      <>
      <FloatingHearts />
        <div className="header-wrapper wrapper">
          <Header />
        </div>
        
        <div className="main-block-wrapper wrapper">
          <MainBlock />
        </div>

        <div className="footer-wrapper">
          <Footer />
        </div> 
      </>
    );
}

export default App;