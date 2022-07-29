import React, { Component } from 'react';
import axios from 'axios';
import MessageModal from './MessageModal.js';
var CryptoJS = require("crypto-js");

export class SignUp extends Component {
    static displayName = SignUp.name;

    constructor(props) {
        super(props);

        this.createUser = this.createUser.bind(this);
        this.onUserInputChange = this.onUserInputChange.bind(this);
        this.onPasswordInputChange = this.onPasswordInputChange.bind(this);
        this.hideModal = this.hideModal.bind(this);

        this.state = {
            userName: "",
            password: "",
            message: "",
            show: false,
            isSuccess: false
        };
    }

    onUserInputChange(event) {
        this.setState({
            userName: event.target.value
        });
    }

    onPasswordInputChange(event) {
        this.setState({
            password: event.target.value
        });
    }

    createUser() {
        if (this.state.password.length > 15) {
            this.setState({
                message: "Password cannot be more of 15 characters.",
                show: true,
                isSuccess: false
            });
        }

        if (this.state.userName.length > 20) {
            this.setState({
                message: "User name cannot be more of 20 characters.",
                show: true,
                isSuccess: false
            });
        }

        if (this.state.userName === "" || this.state.password === "") {
            this.setState({
                show: true,
                message: "Username or password cannot be empty"
            });
        }

        if (this.state.userName !== "" && this.state.password !== "" && this.state.password.length <= 15 &&
            this.state.userName.length <= 20) {
            const encryptedPassword = CryptoJS.AES.encrypt(this.state.password, '/B?E(H+MbQeShVmY').toString();

            axios.post('User/SignUp', {
                UserName: this.state.userName,
                UserPassword: encryptedPassword
            }).then((success) => {
                this.setState({
                    message: success.data,
                    show: true,
                    isSuccess: (success.data.indexOf("already in use") !== -1 ? false : true)
                });
            }).catch((error) => {
                let errorMessage = "Server could not be contacted, try in some minutes";

                if (error.response.data.title === "One or more validation errors occurred.") {
                    errorMessage = "Username is invalid, only allowed numbers, letters and scores.";
                }

                this.setState({
                    message: errorMessage,
                    show: true,
                    isSuccess: false
                });
            });
        }
    }

    hideModal() {
        this.setState({ show: false });
    };

    render() {
        return (
            <section id="signup" className="login">
                <div className="title">
                    <h1>Sign Up</h1>
                </div>
                <div className="form">
                    <input
                        type="text"
                        placeholder="Username"
                        className="text"
                        name="username"
                        value={this.state.userName}
                        onChange={this.onUserInputChange}
                        required>
                    </input>
                    <br></br>
                    <input
                        type="password"
                        placeholder="Password"
                        className="password"
                        name="password"
                        value={this.state.password}
                        onChange={this.onPasswordInputChange}
                        required>
                    </input>
                    <br></br>
                    <button
                        type="submit"
                        id="login-button"
                        className="login-button"
                        onClick={this.createUser}>
                        Register
                    </button>
                    <a href="/" className="forgot-password">
                        Return to login
                    </a>
                </div>
                {this.state.show &&
                    <MessageModal handleClose={this.hideModal} isSuccess={this.state.isSuccess}>
                        <p className="modal-text">
                            {this.state.message}
                        </p>
                    </MessageModal>
                }
            </section>
        );
    }
}
