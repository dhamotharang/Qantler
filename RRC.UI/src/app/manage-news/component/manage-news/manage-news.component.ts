import { Component, OnInit, TemplateRef, ViewChild, Renderer2, Inject } from '@angular/core';
import { CommonService } from 'src/app/common.service';
import { ArabicDataService } from 'src/app/arabic-data.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { DOCUMENT } from '@angular/common';
import { ManagenewsService } from '../../service/managenews.service';
import { SuccessComponent } from 'src/app/modal/success-popup/success.component';

@Component({
  selector: 'app-manage-news',
  templateUrl: './manage-news.component.html',
  styleUrls: ['./manage-news.component.scss']
})
export class ManageNewsComponent implements OnInit {
  @ViewChild('deletetemplate') deletetemplate: TemplateRef<any>;
  currentUser: any = JSON.parse(localStorage.getItem('User'));
  rows: Array<any> = [];
  columns: Array<any> = [];
  config: any = {
    paging: true,
    page: 1,
    maxSize: 10,
    itemsPerPage:10,
    totalItems:0,
  };
  filterBy:any = {
    Type:null,
    Value:'',
  };
  newsColumns: Array<any> = [];
  @ViewChild('actionTemplate') actionTemplate: TemplateRef<any>;
  lang:string;
  isShowNewsAdd=false;
  bsModalRef:BsModalRef;
  CanManageNews = false;
  NewsModel = {
    News: '',
    ExpiryDate: Date,
    CreatedBy: '',
    CreatedDateTime: new Date(),
    NewsID: null
  }
  newsList: any = [];
  newsValue: any;
  message: string;
  tinyConfig = {
    plugins: 'powerpaste casechange importcss tinydrive searchreplace directionality visualblocks visualchars fullscreen table charmap hr pagebreak nonbreaking toc insertdatetime advlist lists checklist wordcount tinymcespellchecker a11ychecker imagetools textpattern noneditable help formatpainter permanentpen pageembed charmap tinycomments mentions quickbars linkchecker emoticons',
    language: this.common.language != 'English' ? "ar" : "en",
    menubar: 'file edit view insert format tools table tc help',
    toolbar: 'undo redo | bold italic underline strikethrough | fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist checklist | forecolor backcolor casechange permanentpen formatpainter removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media pageembed template link anchor codesample | a11ycheck ltr rtl | showcomments addcomment',
    toolbar_drawer: 'sliding',
    directionality: this.common.language != 'English' ? "rtl" : "ltr"
  };
  
  constructor(private common: CommonService, public arabicService:ArabicDataService, public modalService: BsModalService, @Inject(DOCUMENT) private document: Document,
  private renderer: Renderer2, public service: ManagenewsService) { 
  }

  ngOnInit() {
    this.lang = this.common.currentLang;
    this.CanManageNews = this.currentUser.CanManageNews;
    if(this.lang == 'ar'){
      this.newsColumns = [
        { name: this.arabicfn('description'), prop: 'News' },
        { name: this.arabicfn('action'), cellTemplate: this.actionTemplate },
      ];
    } else if(this.lang == 'en') {
      this.newsColumns = [
        { name: 'Description', prop: 'News' },
        { name: 'Action', cellTemplate: this.actionTemplate },
      ];
    }
    this.getNews();
  }

  removeWordSpaces(words:string){
    return  words.replace(/\s+/g, '');
  }
  arabicfn(word) {
    return this.common.arabic.words[word];
  }

  showNewsAdd() {
    this.isShowNewsAdd = true;
  }

  showFilter() {
    this.NewsModel.News = '';
    this.isShowNewsAdd = false;
  }

  closemodal(){
    this.modalService.hide(1);
    this.renderer.removeClass(this.document.body, 'modal-open');
  }

  validationForm() {
    let flag = true;
    if(this.NewsModel.News.trim() !='') {
      flag = false;
    }
    return flag;
  }

  saveNews() {
    if(this.newsValue) {
      this.NewsModel.NewsID = this.newsValue
      this.service.updateNews(this.NewsModel).subscribe(data => {
        this.NewsModel.News = ''
        if (data) {
          if (this.lang === 'en') {
            this.message = 'News Updated Successfully';
          } else {
            this.message = this.common.arabic.words['newsupdatedsuccessfully'];
          }
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.closeDialog();
          });
        }
        this.getNews();
      })
    }else {
      this.service.saveNews(this.NewsModel).subscribe(data => {
        this.NewsModel.News = ''
        if (data) {
          
          if (this.lang === 'en') {
            this.message = 'News Added Successfully';
          } else {
            this.message = this.common.arabic.words['newsaddedsuccessfully'];
          }
          this.bsModalRef = this.modalService.show(SuccessComponent);
          this.bsModalRef.content.message = this.message;
          let newSubscriber = this.modalService.onHide.subscribe(() => {
            newSubscriber.unsubscribe();
            this.closeDialog();
          });
        }
        this.getNews();
      })
  
    }
  }

  getNews() {
    this.service.getNews(this.config.page, this.config.maxSize, this.currentUser.id,this.filterBy.Value).subscribe(data => {
      this.newsList = data;
      this.rows = this.newsList.collection;
      this.config.totalItems = this.newsList.count;
      // this.showFilter();
    })
  }

  viewData(type, value) {
    this.newsValue = value.NewsID;
    this.service.getNewsById(this.currentUser.id, value.NewsID).subscribe(data => {
      this.showNewsAdd();
      this.newsList = data;
      this.NewsModel.News = this.newsList.News;
    })
  }

  openDeleteDialog(value) {
    this.newsValue = value.NewsID;
    this.bsModalRef = this.modalService.show(this.deletetemplate);
  }

  delateNews(value) {
    this.service.deleteNewsById(this.newsValue).subscribe(data => {
      if(data == "True") {
        this.getNews();
        this.closeDialog();
      }
    })
  }

  closeDialog() {
    this.bsModalRef.hide();
  }
  public onChangePage(config: any, page: any = { page: this.config.page, itemsPerPage: this.config.itemsPerPage }): any {
    this.config.page = page;
    this.getNews();
  }
}
