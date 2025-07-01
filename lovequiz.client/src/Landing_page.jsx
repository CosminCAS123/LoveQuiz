import { useNavigate } from 'react-router-dom';

import { useEffect, useState } from 'react';

import React from 'react';
import './main-block.scss';
import './tailwind.css';


function MainBlock() {
    const navigate = useNavigate();

    return(
        <div className="main-block flex pb-12 -z-20">
            <div className="main-block-text flex-1/2 items-center justify-start pl-12">
                <div className="animate-slide-down main-block__title z-10">
                    <div className="main-block__title">
                        <em>TU ESTI DE VINA!</em>
                    </div>
                    <div className="main-block__title--subtitle pl-3">
                        <em>sau poate sunt eu...?</em>
                    </div>
                </div>

                <div class="block px-4 paragraph-block mb-8 animate-fade-in mt-8 z-10">
                    <span className="block mb-6">Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</span>
                    <span className="block">Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</span>
                </div>

                <div className="animate-slide-up main-block__button-wrapper wrapper self-start z-10">
                    <button className="main-block__button" onClick={() => navigate('/gender')}>
                        Afla raspunsul acum!
                        <span className="block">*in doar 5 minute*</span>
                    </button>
                </div>
            </div>

            <div className="animate-grow main-block__image wrapper main-block-image flex-1/2 z-10">
                <img src="../assets/main-img.png" className="main-block__image--main"/>
            </div>

        </div>
    );
}

export default MainBlock;