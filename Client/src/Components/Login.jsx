import { useState } from 'react';
import { toast } from 'react-toastify';

export default function Login({ fetchUser, showLogin, setShowLogin, setShowRegistration }) {
    const [login, setLogin] = useState({ email: '', password: '' });

    const handleOverlayClick = (e) => {
        if (e.target.classList.contains('login')) {
            setShowLogin(false);
            setTimeout(() => {
                setLogin({ email: '', password: '' });
            }, 1000);
        }
    };

    const handleClose = () => {
        setShowLogin(false);
        setTimeout(() => {
            setLogin({ email: '', password: '' });
        }, 1000);

    }

    const handleShowRegister = () => {
        setShowRegistration(true);
        setShowLogin(false);
    }

    const handleLogin = async e => {
        e.preventDefault();

        try {
            const loginRes = await fetch('/api/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ email: login.email, password: login.password })
            });

            const loginData = await loginRes.json();

            if (loginRes.ok) {
                setShowLogin(false);
                fetchUser();
                setTimeout(() => {
                    setLogin({ email: '', password: '' });
                }, 1000);

                toast.success(loginData.message, {
                    position: "top-right",
                    autoClose: 3000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                    theme: "colored",
                });
            }
            else if (loginRes.status === 401) {
                toast.warn(loginData.message, {
                    position: "top-right",
                    autoClose: 4000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                    theme: "colored",
                });
            }
            else {
                toast.error(loginData.message, {
                    position: "top-right",
                    autoClose: 3000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                    theme: "colored",
                });
            }
        } catch (error) {
            console.log(error);
        }
    }

    return showLogin ?
        (<div className={'login active'} onClick={handleOverlayClick}>
            <div className={'login-form active'}>
                <button className='login-close-button' onClick={handleClose}>&times;</button>
                <div className='login-header'>
                    <h1>Login</h1>
                </div>
                <div className='login-body'>
                    <input type="text" placeholder="Email" value={login.email} onChange={e => setLogin({ ...login, email: e.target.value })} />
                    <input type="password" placeholder="Password" value={login.password} onChange={(e) => setLogin({ ...login, password: e.target.value })} />
                </div>
                <div className='login-footer'>
                    <button className='login-button' onClick={handleLogin}>Login</button>
                    <button className='registration-button' onClick={handleShowRegister}>Registration</button>
                </div>
            </div>
        </div>)
        :
        (<div className={'login'} onClick={handleOverlayClick}>
            <div className={'login-form'}>
                <button className='login-close-button' onClick={handleClose}>x</button>
                <div className='login-header'>
                    <h1>Login</h1>
                </div>
                <div className='login-body'>
                    <input type="text" placeholder="Email" value={login.email} onChange={e => setLogin({ ...login, email: e.target.value })} />
                    <input type="password" placeholder="Password" value={login.password} onChange={(e) => setLogin({ ...login, password: e.target.value })} />
                </div>
                <div className='login-footer'>
                    <button className='login-button' onClick={handleLogin}>Login</button>
                    <button className='registration-button' onClick={handleShowRegister}>Registration</button>
                </div>
            </div>
        </div>);
}