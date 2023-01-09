import { useState } from 'react';
import { Dialog } from '@headlessui/react';
import React from 'react';

export default function ConfirmModal({
    title,
    description,
    text, 
     warn,
     buttonText,
     action
  }: {
    title: string;
    description: string;
    text: string;
    warn: boolean;
    buttonText: string;
    action: Function;
  }) {
  let [isOpen, setIsOpen] = useState(true);
  let buttonClass="";
  if( warn)
    buttonClass+="btn-warning";

  return (
    <Dialog open={isOpen} onClose={() => setIsOpen(false)}>
      <Dialog.Panel>
        <Dialog.Title>{title}</Dialog.Title>
        <Dialog.Description>
        {description}
        </Dialog.Description>
        <p>
            {warn&&(
        <svg xmlns="http://www.w3.org/2000/svg" className="stroke-current flex-shrink-0 h-6 w-6" fill="none" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" /></svg>
            )}
            {text}
        </p>
        <button className={buttonClass} onClick={() => {setIsOpen(false); action();}}>{buttonText}</button>
        
        <button onClick={() => setIsOpen(false)}>Cancel</button>
      </Dialog.Panel>
    </Dialog>
  )
}