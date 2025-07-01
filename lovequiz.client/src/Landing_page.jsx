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
        <div className="main-block flex pb-12">
            <div className="main-block-text flex-1/2 items-center justify-start pl-12">
                <div className="animate-slide-down main-block__title ">
                    <div className="main-block__title">
                        <em>TU ESTI DE VINA!</em>
                    </div>
                    <div className="main-block__title--subtitle pl-3">
                        <em>sau poate sunt eu...?</em>
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
                <div class="block px-4 paragraph-block mb-8 animate-fade-in mt-8">
                    <span className="block mb-6">Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</span>
                    <span className="block">Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</span>
                </div>

                <div className="animate-slide-up main-block__button-wrapper wrapper self-start">
                    <button className="main-block__button" onClick={() => navigate('/gender')}>
                        Afla raspunsul acum!
                        <span className="block">*in doar 5 minute*</span>
                    </button>
                </div>
            </div>




            <div className="animate-grow main-block__image wrapper main-block-image flex-1/2">
                <img src="../assets/main-img.png" className="main-block__image--main"/>
            </div>

        </div>
    );
}

export default MainBlock;