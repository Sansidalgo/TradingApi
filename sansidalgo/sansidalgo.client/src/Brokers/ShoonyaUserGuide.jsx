import React from 'react';

function ShoonyaUserGuide() {
    return (

        <section className="maincontent">
          
             
                    <div className="col-lg-4 col-md-5 offset-md-5">
                        <div className="heading_container">
                            <h2>Shoonya User Guide</h2>
                        </div>
                    </div>
             
                
            <div className="col-lg-12 col-md-2 offset-md-2">


                        <div class="myDiv">
                            <h2>Steps to follow</h2>

                        </div>
                        <div>
                            <p>1.	Login into your finvasia account, click on your profile to open the slide.</p>
                        </div>
                        <div >
                            <img height="600" width="1150" src="Imgs\Login.png" alt="Italian Trulli" />
                        </div>
                        <div>
                            <p>2.	Click on account on left top position.</p></div>

                        <div >
                            <img height="600" width="1150" src="Imgs\key.png" alt="Italian Trulli" />
                        </div>
                        <div>
                            3.	Expand Security link and click on TOTP
                        </div>
                        <div>
                            <img height="600" width="1150" src="Imgs\authkey.png" alt="Italian Trulli" />
                        </div>
                        <div>
                            4. Download Microsoft authenticator app in your mobile to set the set up, scan the finvasia QR code
                        </div>
                        <div>
                            <img height="600" width="1150" src="Imgs\authApp.png" alt="Italian Trulli" />
                        </div>

                    </div>
               
         
        </section>


    );
}

export default ShoonyaUserGuide;