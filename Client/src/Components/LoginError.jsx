export default function LoginError({ setShowLogin }) {
    const handleLogin = () => {
        setShowLogin(true);
    }

    return (
        <div className='login-error'>
            <h1>You have to login to use this function</h1>
            <button className='login-button' onClick={handleLogin}>Login</button>
        </div>
    );
}