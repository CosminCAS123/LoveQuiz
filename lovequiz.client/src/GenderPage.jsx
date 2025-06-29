import React from 'react';
import './App.css';
import './GenderBlock.scss';
import Header from './Header.jsx'
import Footer from './Footer.jsx'

function GenderBlock() {
    const handleClick = () => {
        alert('Salut! Ai apasat butonul 🚀');
    };

    return (
      <>
        <div className="header-wrapper wrapper">
          <Header />
        </div>

        <div className="gender-block-wrapper wrapper ccontainer">
            <div className="gender-block">
                <div className="gender-block__title">
                    Alege-ti genul:
                </div>

                <div className="gender-block__genders">
                    <button className="gender-block__genders-box">
                        <img src="../assets/man-3.png" className="gender-block__genders--male-image"/>

                        <span className="gender-block__genders--male-text">Barbat</span>
                    </button>
                    <button className="gender-block__genders-box">
                        <img src="../assets/woman-2.png" className="gender-block__genders--female-image"/>

                        <span className="gender-block__genders--female-text">Femeie</span>
                    </button>
                </div>
            </div>
        </div>

        <div className="footer-wrapper">
          <Footer />
        </div> 
      </>
    );
}

export default GenderBlock;