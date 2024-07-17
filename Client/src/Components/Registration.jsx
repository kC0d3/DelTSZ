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

    const handleOverlayClick = (e) => {
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

    const handleClose = () => {
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

    const handleCancel = () => {
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

    return showRegistration ?
        (<div className='registration active' onClick={handleOverlayClick}>
            <div className={'registration-form active'}>
                <button className='registration-close-button' onClick={handleClose}>&times;</button>
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
                    <button className='registration-button' onClick={handleRegistration}>Registration</button>
                    <button className='registration-cancel-button' onClick={handleCancel}>Cancel</button>
                </div>
            </div>
        </div>)
        :
        (<div className='registration' onClick={handleOverlayClick}>
            <div className={'registration-form'}>
                <button className='registration-close-button' onClick={handleClose}>x</button>
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
                    <button className='registration-button' onClick={handleRegistration}>Registration</button>
                    <button className='registration-cancel-button' onClick={handleCancel}>Cancel</button>
                </div>
            </div>
        </div>);
}