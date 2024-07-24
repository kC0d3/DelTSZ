import { useState, useRef } from 'react';
import { Link } from 'react-router-dom';
import { toast } from 'react-toastify';

export default function ProfileMenu({ loggedUser, setLoggedUser }) {
    const [open, setOpen] = useState(false);
    const menuRef = useRef(null);

    const handleLogout = async () => {
        try {
            const logoutRes = await fetch('/api/auth/logout', {
                method: 'GET'
            });
            const logoutData = await logoutRes.json();

            if (logoutRes.ok) {
                setLoggedUser(undefined);
                toast.info(logoutData.message, {
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
            setOpen(false);
        } catch (error) {
            console.log(error);
            setLoggedUser(undefined);
        }
    }

    const handleOpen = () => {
        setOpen(!open);
    }

    const handleBlur = (e) => {
        if (menuRef.current && !menuRef.current.contains(event.relatedTarget)) {
            setOpen(false);
        }
    };

    return (
        <>
            <Link onClick={handleOpen} onBlur={handleBlur} ref={menuRef} tabIndex={0}>{loggedUser.username}</Link >
            {open ?
                <div className='profile-menu open' onBlur={handleBlur} ref={menuRef} tabIndex={0}>
                    {
                        loggedUser.role === 'Owner' &&
                        <>
                            <Link className='receive-ingredient-link' to='/ingredients/receive' onClick={handleOpen}>Receive ingredient</Link>
                            <Link className='create-product-link' to='/products/create' onClick={handleOpen}>Create product</Link>
                        </>

                    }
                    {
                        loggedUser.role === 'Producer' &&
                        <Link className='provide-ingredient-link' to='/ingredients/provide' onClick={handleOpen}>Provide ingredients</Link>
                    }
                    {
                        loggedUser.role === 'Customer' &&
                        <Link className='order-products-link' to='/products' onClick={handleOpen}>Order Products</Link>
                    }
                    <Link className='profile-link' to='/profile' onClick={handleOpen}>Profile</Link>
                    <Link className='logout-link' to='/' onClick={handleLogout}>Logout</Link>
                </div>
                :
                <div className='profile-menu' onBlur={handleBlur} ref={menuRef} tabIndex={0}>
                    {
                        loggedUser.role === 'Owner' &&
                        <>
                            <Link className='receive-ingredient-link' to='/ingredients/receive' onClick={handleOpen}>Receive ingredient</Link>
                            <Link className='create-product-link' to='/products/create' onClick={handleOpen}>Create product</Link>
                        </>

                    }
                    {
                        loggedUser.role === 'Producer' &&
                        <Link className='provide-ingredient-link' to='/ingredients/provide' onClick={handleOpen}>Provide ingredients</Link>
                    }
                    {
                        loggedUser.role === 'Customer' &&
                        <Link className='order-products-link' to='/products' onClick={handleOpen}>Order Products</Link>
                    }
                    <Link className='profile-link' to='/profile' onClick={handleOpen}>Profile</Link>
                    <Link className='logout-link' to='/' onClick={handleLogout}>Logout</Link>
                </div>
            }
        </>
    );
}