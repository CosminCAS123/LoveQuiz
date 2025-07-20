import { useNavigate } from 'react-router-dom';

import { useEffect, useState } from 'react';

import React from 'react';
import './main-block.scss';
import './tailwind.css';
import FloatingHearts from './FloatingHeats';


function MainBlock() {
    const navigate = useNavigate();

    return(
        <>
        <div>
            <FloatingHearts />
        </div>
        
        <div className="main-block flex pb-12">
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
                    <span className="block mb-6">Relațiile toxice nu sunt întotdeauna ușor de recunoscut. Uneori se ascund sub forma "iubirii pasionale", "grijii" sau "dependenței emoționale". Fie că e vorba de manipulare, control, gelozie sau vinovăție constantă – toate lasă urme. Răspunde sincer la întrebări și descoperă ce se ascunde în spatele sentimentelor tale.</span>
                    <span className="block">Meriți o iubire care te ridică, nu una care te frânge.
Dacă ai ajuns să te întrebi constant „e vina mea?” sau „de ce nu sunt suficient(ă)?”, e timpul să te oprești și să privești lucrurile cu claritate.
Acest quiz nu îți va da toate răspunsurile, dar îți poate oferi un punct de plecare.
Începe acum. Pentru tine.</span>
                </div>

                <div className="animate-slide-up main-block__button-wrapper wrapper self-start">
                    <button className="main-block__button" onClick={() => navigate('/gender')}>
                        Afla raspunsul acum!
                        <span className="block">*in doar 5 minute*</span>
                    </button>
                </div>
            </div>

            <div className="animate-grow main-block__image wrapper main-block-image flex-1/2 z-10">
                <img src="/assets/main-img.png" className="main-block__image--main"/>
            </div>

        </div>
        </>
    );
}

export default MainBlock;