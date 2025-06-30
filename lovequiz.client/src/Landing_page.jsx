import { useNavigate } from 'react-router-dom';

import { useEffect, useState } from 'react';

import React from 'react';
import './main-block.scss';
import './tailwind.css';


function MainBlock() {
    const navigate = useNavigate();
    

    const [question, setQuestion] = useState(null);   //api stuff
    const [error,    setError]    = useState(null);

    useEffect(() => {
        fetch('http://localhost:5023/api/quiz/questions')   // adjust port
          .then(r => {
            if (!r.ok) throw new Error('HTTP ' + r.status);
            return r.json();
          })
          .then(data => setQuestion(data[1]))               // just take first
          .catch(setError);
    }, []);

    return(
        <div className="main-block">
            <div className="animate-slide-down main-block__title">
                <div className="main-block__title--curved">
                    <svg className="mx-auto" width="100%" viewBox="0 0 1000 160">
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


            {/* <div className="main-block__quiz">
                {error && <p className="error">Nu se poate încărca întrebarea.</p>}

                {!error && !question && <p>Se încarcă…</p>}

                {question && (
                <>
                    <h2 className="question">{question.question}</h2>
                    <ul className="answers">
                    {question.answers.map(ans => (
                        <li key={ans.id}>
                        <label>
                            <input type="radio" name="q1" value={ans.id} />
                            {ans.answer}
                        </label>
                        </li>
                    ))}
                    </ul>
                </>
                )}
            </div> */}




            <div className="animate-grow main-block__image wrapper">
                <img src="../assets/toxicity.png" className="main-block__image--main"/>
            </div>

            <div className="animate-slide-up main-block__button-wrapper wrapper">
                <button className="main-block__button" onClick={() => navigate('/gender')}>
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