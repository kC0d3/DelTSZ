import { useState } from 'react';

export default function Login({ setLoggedUser, showLogin, setShowLogin, setShowRegistration }) {
    const [login, setLogin] = useState({ username: '', password: '' });

    const handleOverlayClick = (e) => {
        if (e.target.classList.contains('login')) {
            setShowLogin(false);
        }
    };

    const handleClose = () => {
        setShowLogin(false);
    }

    const handleShowRegister = () => {
        setShowRegistration(true);
        setShowLogin(false);
    }

    return showLogin ?
        (<div className={'login active'} onClick={handleOverlayClick}>
            <div className={'login-form active'}>
                <button className='login-close' onClick={handleClose}>&times;</button>
                <div className='login-header'>
                    <h1>Login</h1>
                </div>
                <div className='login-body'>
                    <input type="text" placeholder="Email" value={login.username} onChange={e => setLogin({ ...login, username: e.target.value })} />
                    <input type="password" placeholder="Password" value={login.password} onChange={(e) => setLogin({ ...login, password: e.target.value })} />
                </div>
                <div className='login-footer'>
                    <button className='login-button'>Login</button>
                    <button className='registration-button' onClick={handleShowRegister}>Registration</button>
                </div>
            </div>
        </div>)
        :
        (<div className={'login'} onClick={handleOverlayClick}>
            <div className={'login-form'}>
                <button className='login-close' onClick={handleClose}>x</button>
                <div className='login-header'>
                    <h1>Login</h1>
                </div>
                <div className='login-body'>
                    <input type="text" placeholder="Email" value={login.username} onChange={e => setLogin({ ...login, username: e.target.value })} />
                    <input type="password" placeholder="Password" value={login.password} onChange={(e) => setLogin({ ...login, password: e.target.value })} />
                </div>
                <div className='login-footer'>
                    <button className='login-button'>Login</button>
                    <button className='registration-button' onClick={handleShowRegister}>Registration</button>
                </div>
            </div>
        </div>);
}