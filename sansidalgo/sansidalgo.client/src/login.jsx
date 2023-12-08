import React from 'react'
import ReactDOM from 'react-dom/client'
import { useEffect, useState } from 'react';

function login() {
    return ( /*< !--login section-- >*/
  <section className="contact_section layout_padding-top">
    <div className="container-fluid">
      <div className="row">
        <div className="col-lg-4 col-md-5 offset-md-1">
          <div className="heading_container">
            <h2>
              Login
            </h2>
          </div>
        </div>
      </div>
      <div className="row">
        <div className="col-lg-6 col-md-11 offset-md-1">
          <div className="form_container">
            <form action="">
              <div>
                <input type="email" placeholder="Email Id" />
              </div>
            <div>
                <input type="text" placeholder="OR" disabled />
            </div>
              <div>
                <input type="text" placeholder="Phone Number" />
              </div>
              <div>
                <input type="password" placeholder="password" />
              </div>
             
              <div className="btn_box" >
                <button>
                  Login
                </button>
              </div>
              <div className="second-div">

              </div>
            </form>
          </div>
        </div>
        
      </div>
    </div>
  </section>
/*  <!--end login section-- >*/
);
}
export default login;