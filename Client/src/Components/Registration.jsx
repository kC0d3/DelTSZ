import { useState } from 'react';
import { toast } from 'react-toastify';

export default function Registration({ showRegistration, setShowRegistration, setShowLogin }) {
    const [registration, setRegistration] = useState({
        email: '',
        username: '',
        companyname: '',
        address: {
            zipcode: '',
            city: '',
            street: '',
            housenumber: ''
        },
        password: '',
        confirmpassword: ''
    });

    const handleOverlayClick = e => {
        e.preventDefault();
        if (e.target.classList.contains('registration')) {
            setShowRegistration(false);
            toast.dismiss();
            setTimeout(() => {
                setRegistration({
                    email: '',
                    username: '',
                    companyname: '',
                    address: {
                        zipcode: '',
                        city: '',
                        street: '',
                        housenumber: ''
                    },
                    password: '',
                    confirmpassword: ''
                });
            }, 1000);
        }
    };

    const handleClose = e => {
        e.preventDefault();
        setShowRegistration(false);
        toast.dismiss();
        setTimeout(() => {
            setRegistration({
                email: '',
                username: '',
                companyname: '',
                address: {
                    zipcode: '',
                    city: '',
                    street: '',
                    housenumber: ''
                },
                password: '',
                confirmpassword: ''
            });
        }, 1000);
    }

    const handleCancel = e => {
        e.preventDefault();
        setShowRegistration(false);
        setShowLogin(true);
        toast.dismiss();
        setTimeout(() => {
            setRegistration({
                email: '',
                username: '',
                companyname: '',
                address: {
                    zipcode: '',
                    city: '',
                    street: '',
                    housenumber: ''
                },
                password: '',
                confirmpassword: ''
            });
        }, 1000);
    }

    const handleRegistration = async e => {
        e.preventDefault();

        try {
            const regRes = await fetch('/api/auth/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    email: registration.email,
                    username: registration.username,
                    companyname: registration.companyname,
                    address: {
                        zipcode: registration.address.zipcode,
                        city: registration.address.city,
                        street: registration.address.street,
                        housenumber: registration.address.housenumber
                    },
                    password: registration.password,
                    confirmpassword: registration.confirmpassword
                })
            });

            const regData = await regRes.json();
            notifyAndActByResponse(regRes, regData);
        }
        catch (error) {
            console.log(error);
        }
    }

    const notifyAndActByResponse = (regRes, regData) => {
        if (regRes.ok) {
            setShowRegistration(false);
            setTimeout(() => {
                setRegistration({
                    email: '',
                    username: '',
                    companyname: '',
                    address: {
                        zipcode: '',
                        city: '',
                        street: '',
                        housenumber: ''
                    },
                    password: '',
                    confirmpassword: ''
                });
            }, 1000);

            toast.dismiss();
            setTimeout(() => {
                toast.success(regData.message, {
                    position: "top-right",
                    autoClose: 3000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                    theme: "colored",
                });
            }, 600);
        }
        else if (regRes.status === 409) {
            toast.dismiss();
            setTimeout(() => {
                toast.warn(
                    <div className='warning-messages'>
                        {regData.errors.map((error, index) => (
                            <div className='warning-message' key={index}>{error.description}</div>
                        ))}
                    </div>, {
                    position: "top-right",
                    autoClose: false,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                    theme: "colored",
                });
            }, 600);
        }
        else if (regRes.status === 400 && regData.errors) {
            toast.dismiss();
            setTimeout(() => {
                toast.warn(
                    regData.errors.ConfirmPassword[0], {
                    position: "top-right",
                    autoClose: 5000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                    theme: "colored",
                });
            }, 600);
        }
        else {
            toast.dismiss();
            setTimeout(() => {
                toast.error(
                    regData.message, {
                    position: "top-right",
                    autoClose: 5000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                    theme: "colored",
                });
            }, 600);
        }
    }

    return showRegistration ?
        (<form className='registration active' onClick={handleOverlayClick} onSubmit={handleRegistration}>
            <div className={'registration-form active'}>
                <button type='button' className='registration-close-button' onClick={handleClose}>&times;</button>
                <div className='registration-header'>
                    <h1>Registration</h1>
                </div>
                <div className='registration-body'>
                    <div className='registration-body-top'>
                        <div className='registration-personal-data'>
                            <input type="text" placeholder="Email" value={registration.email} onChange={e => setRegistration({ ...registration, email: e.target.value })} />
                            <input type="text" placeholder="Username" value={registration.username} onChange={e => setRegistration({ ...registration, username: e.target.value })} />
                            <input type="text" placeholder="Company name" value={registration.companyname} onChange={e => setRegistration({ ...registration, companyname: e.target.value })} />
                        </div>
                        <div className='registration-address-data'>
                            <input type="text" placeholder="Zip code" value={registration.address.zipcode} onChange={e => setRegistration({ ...registration, address: { ...registration.address, zipcode: e.target.value } })} />
                            <input type="text" placeholder="City" value={registration.address.city} onChange={e => setRegistration({ ...registration, address: { ...registration.address, city: e.target.value } })} />
                            <input type="text" placeholder="Street" value={registration.address.street} onChange={e => setRegistration({ ...registration, address: { ...registration.address, street: e.target.value } })} />
                            <input type="text" placeholder="House number" value={registration.address.housenumber} onChange={e => setRegistration({ ...registration, address: { ...registration.address, housenumber: e.target.value } })} />
                        </div>
                    </div>
                    <div className='registration-body-bottom'>
                        <div className='registration-password-data'>
                            <input type="password" placeholder="Password" value={registration.password} onChange={e => setRegistration({ ...registration, password: e.target.value })} />
                            <input type="password" placeholder="Confirm password" value={registration.confirmpassword} onChange={e => setRegistration({ ...registration, confirmpassword: e.target.value })} />
                        </div>
                    </div>
                </div>
                <div className='registration-footer'>
                    <button type='submit' className='registration-button'>Registration</button>
                    <button type='button' className='registration-cancel-button' onClick={handleCancel}>Cancel</button>
                </div>
            </div>
        </form >)
        :
        (<form className='registration' onClick={handleOverlayClick} onSubmit={handleRegistration} >
            <div className={'registration-form'}>
                <button type='button' className='registration-close-button' onClick={handleClose}>x</button>
                <div className='registration-header'>
                    <h1>Registration</h1>
                </div>
                <div className='registration-body'>
                    <div className='registration-body-top'>
                        <div className='registration-personal-data'>
                            <input type="text" placeholder="Email" value={registration.email} onChange={e => setRegistration({ ...registration, email: e.target.value })} />
                            <input type="text" placeholder="Username" value={registration.username} onChange={e => setRegistration({ ...registration, username: e.target.value })} />
                            <input type="text" placeholder="Company name" value={registration.companyname} onChange={e => setRegistration({ ...registration, companyname: e.target.value })} />
                        </div>
                        <div className='registration-address-data'>
                            <input type="text" placeholder="Zip code" value={registration.address.zipcode} onChange={e => setRegistration({ ...registration, address: { ...registration.address, zipcode: e.target.value } })} />
                            <input type="text" placeholder="City" value={registration.address.city} onChange={e => setRegistration({ ...registration, address: { ...registration.address, city: e.target.value } })} />
                            <input type="text" placeholder="Street" value={registration.address.street} onChange={e => setRegistration({ ...registration, address: { ...registration.address, street: e.target.value } })} />
                            <input type="text" placeholder="House number" value={registration.address.housenumber} onChange={e => setRegistration({ ...registration, address: { ...registration.address, housenumber: e.target.value } })} />
                        </div>
                    </div>
                    <div className='registration-body-bottom'>
                        <div className='registration-password-data'>
                            <input type="password" placeholder="Password" value={registration.password} onChange={e => setRegistration({ ...registration, password: e.target.value })} />
                            <input type="password" placeholder="Confirm password" value={registration.confirmpassword} onChange={e => setRegistration({ ...registration, confirmpassword: e.target.value })} />
                        </div>
                    </div>
                </div>
                <div className='registration-footer'>
                    <button type='submit' className='registration-button'>Registration</button>
                    <button type='button' className='registration-cancel-button' onClick={handleCancel}>Cancel</button>
                </div>
            </div>
        </form >);
}