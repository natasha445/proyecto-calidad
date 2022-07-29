import React from 'react';
import './MessageModal.css';
import warning from './icons/warning.png';
import success from './icons/success.png';

const MessageModal = ({ handleClose, isSuccess, fromAgenda, children }) => {
    const modalClassName = fromAgenda ? "modal-main-agenda" : "modal-main"
    const buttonSuccessClassName = isSuccess ? "close-button-success" : "close-button";

    return (
        <div className="error-modal">
            <section className={modalClassName}>
                <img src={isSuccess ? success : warning} className="icon" />
                {children}
                <button className={buttonSuccessClassName} type="button" onClick={handleClose}>
                Close
            </button>
            </section>
        </div>
    );
};

export default MessageModal;