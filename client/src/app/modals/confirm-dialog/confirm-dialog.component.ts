import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css'],
})
export class ConfirmDialogComponent implements OnInit {
  @Input() internalData;
  title: string = '';
  message: string = '';
  btnOkText: string = '';
  btnCancelText: string = '';
  result: boolean = false;

  constructor(public activeModal: NgbActiveModal) {}

  ngOnInit(): void {
    this.title = this.internalData.title;
    this.message = this.internalData.message;
    this.btnOkText = this.internalData.btnOkText;
    this.btnCancelText = this.internalData.btnCancelText;
  }

  confirm() {
    this.result = true;
    this.activeModal.close();
  }

  decline() {
    this.result = false;
    this.activeModal.close();
  }
}
