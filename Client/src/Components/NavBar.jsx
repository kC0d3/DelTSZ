import { Link } from 'react-router-dom';

export default function NavBar({ loggedUser, setLoggedUser, setShowLogin }) {

    const handleLogout = async () => {
        try {
            await fetch('/api/auth/logout', {
                method: 'GET'
            });
            setLoggedUser(undefined);
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
                                <Link to='/products/create'>Create product</Link>
                            </>
                        ) : loggedUser.role === 'Producer' ? (
                            <>
                                <Link to='/ingredients/create'>Provide ingredients</Link>
                                <Link to='/profile'>Profile</Link>
                            </>
                        ) : (
                            <>
                                <Link to='/products'>Order Products</Link>
                            </>
                        )}
                        <Link to='/profile'>Profile</Link>
                        <Link to='/' onClick={handleLogout}>Logout</Link>
                    </>
                ) : (
                    <Link className='login-link' onClick={handleShowLogin}>Login</Link>
                )}
            </div>
        </div>
    );
}