import { Link } from 'react-router-dom';
import { toast } from 'react-toastify';

export default function NavBar({ loggedUser, setLoggedUser, setShowLogin }) {

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
        } catch (error) {
            console.log(error);
        }
    }

    const handleShowLogin = () => {
        setShowLogin(true);
    }

    return (
        <div className='navbar'>
            <Link to='/' className='logo-link'><img className='navbar-logo' src='/deltsz.ico' alt='logo' /></Link>
            <div className='navbar-links'>
                <Link className='about-us-link' to='/about-us'>About us</Link>
                <Link className='our-products-link' to='/our-products'>Our Products</Link>
                {loggedUser ? (
                    <>
                        {loggedUser.role === 'Owner' ? (
                            <>
                                <Link className='create-product-link' to='/products/create'>Create product</Link>
                            </>
                        ) : loggedUser.role === 'Producer' ? (
                            <>
                                <Link className='provide-ingredient-link' to='/ingredients/create'>Provide ingredients</Link>
                            </>
                        ) : (
                            <>
                                <Link className='order-products-link' to='/products'>Order Products</Link>
                            </>
                        )}
                        <Link className='profile-link' to='/profile'>{loggedUser.username}</Link>
                        <Link className='logout-link' to='/' onClick={handleLogout}>Logout</Link>
                    </>
                ) : (
                    <Link className='login-link' onClick={handleShowLogin}>Login</Link>
                )}
            </div>
        </div>
    );
}