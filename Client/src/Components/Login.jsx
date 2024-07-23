import { useState } from 'react';
import { toast } from 'react-toastify';

export default function Login({ fetchUser, showLogin, setShowLogin, setShowRegistration }) {
    const [login, setLogin] = useState({ email: '', password: '' });

    const handleOverlayClick = e => {
        if (e.target.classList.contains('login')) {
            handleClose(e);
        }
    };

    const handleClose = e => {
        e.preventDefault();
        setShowLogin(false);
        setTimeout(() => {
            setLogin({ email: '', password: '' });
        }, 1000);

    }

    const handleShowRegister = e => {
        e.preventDefault();
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
            notifyAndActByResponse(loginRes, loginData);

        } catch (error) {
            console.log(error);
        }
    }

    const notifyAndActByResponse = (loginRes, loginData) => {
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
    }

    return showLogin ?
        (<form className={'login active'} onClick={handleOverlayClick} onSubmit={handleLogin}>
            <div className={'login-form active'}>
                <button type='button' className='login-close-button' onClick={handleClose}>&times;</button>
                <div className='login-header'>
                    <h1>Login</h1>
                </div>

                <div className='login-body'>
                    <input type="text" placeholder="Email" value={login.email} onChange={e => setLogin({ ...login, email: e.target.value })} />
                    <input type="password" placeholder="Password" value={login.password} onChange={(e) => setLogin({ ...login, password: e.target.value })} />
                </div>
                <div className='login-footer'>
                    <button type='submit' className='login-button'>Login</button>
                    <button type='button' className='registration-button' onClick={handleShowRegister}>Registration</button>
                </div>

            </div>
        </form>)
        :
        (<form className={'login'} onClick={handleOverlayClick} onSubmit={handleLogin}>
            <div className={'login-form'}>
                <button type='button' className='login-close-button' onClick={handleClose}>x</button>
                <div className='login-header'>
                    <h1>Login</h1>
                </div>
                <div className='login-body'>
                    <input type="text" placeholder="Email" value={login.email} onChange={e => setLogin({ ...login, email: e.target.value })} />
                    <input type="password" placeholder="Password" value={login.password} onChange={(e) => setLogin({ ...login, password: e.target.value })} />
                </div>
                <div className='login-footer'>
                    <button type='submit' className='login-button'>Login</button>
                    <button type='button' className='registration-button' onClick={handleShowRegister}>Registration</button>
                </div>
            </div>
        </form>);
}