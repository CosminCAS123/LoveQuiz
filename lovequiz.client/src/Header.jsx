import React from 'react';
import './header.scss';

function Header() {
    return (
        <nav className="header"> 
            <a href="/" className="header__logo">
                <img className="header__logo--image" src="/assets/temporary_logo.png"></img>
            </a>
            {/* <div className="header__title">Love Quiz</div>
            <a className="header__login">Login</a> */}
        </nav>
    );
}

export default Header;