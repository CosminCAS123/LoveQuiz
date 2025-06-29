import React from 'react';
import './main-block.scss'

function MainBlock() {
    return(
        <div className="main-block">
            <div className="main-block__title">
                <div className="main-block__title--curved">
                    <svg width="100%" viewBox="0 0 1000 160">
                        <defs>
                            <path id="curve" d="M 100 200 A 400 100 0 0 1 900 200" />
                        </defs>

                        <text font-size="110" fill="#E94A5A" stroke="#000" stroke-width="4" text-anchor="middle">
                            <textPath href="#curve" startOffset="50%">ESTE VINA TA!</textPath>
                        </text>
                    </svg>
                </div>
                <div className="main-block__title--subtitle">
                    sau e a mea...?
                </div>
            </div>

            <div className="main-block__image wrapper">
                <img src="../assets/toxicity.png" className="main-block__image--main"/>
            </div>

            <div className="main-block__button-wrapper wrapper">
                <button className="main-block__button">
                    Afla raspunsul acum!
                </button>
                <div className="main-block__button--hint">
                    *in doar 5 minute*
                </div>
            </div>
        </div>
    );
}

export default MainBlock;