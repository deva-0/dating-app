import { Injectable } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { ConfirmDialogComponent } from '../modals/confirm-dialog/confirm-dialog.component';

@Injectable({
  providedIn: 'root',
})
export class ConfirmService {
  modalRef: NgbModalRef;

  constructor(private modalService: NgbModal) {}

  confirm(
    title: string = 'Confirmation',
    message: string = 'Are you sure you want to do this?',
    btnOkText: string = 'OK',
    btnCancelText: string = 'Cancel'
  ): Observable<boolean> {
    this.modalRef = this.modalService.open(ConfirmDialogComponent);
    this.modalRef.componentInstance.internalData = {
      title: title,
      message: message,
      btnOkText: btnOkText,
      btnCancelText: btnCancelText,
    };

    return new Observable<boolean>(this.getResult());
  }

  private getResult() {
    return (observer) => {
      const subscription = this.modalRef.closed.subscribe(() => {
        observer.next(this.modalRef.componentInstance.result);
        observer.complete();
      });

      return {
        unsubscribe() {
          subscription.unsubscribe();
        },
      };
    };
  }
}
