export default function ProfileData({ loggedUser }) {

    return (
        <div className='profile-data'>
            <h1>Profile</h1>
            <h2><span>Email: </span>{loggedUser.email}</h2>
            <h2><span>Username: </span>{loggedUser.username}</h2>
            <h2><span>Companyname: </span>{loggedUser.companyname}</h2>
            <h2><span>Role: </span>{loggedUser.role}</h2>
            <h2>
                <span>Address: </span>
                {loggedUser.address.zipcode}, {loggedUser.address.city} {loggedUser.address.street} {loggedUser.address.housenumber}.
            </h2>
        </div>
    );
}