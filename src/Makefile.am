
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG;TRACE"
ASSEMBLY = bin/Debug/Tsunami.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug/

TSUNAMI_EXE_MDB_SOURCE=bin/Debug/Tsunami.exe.mdb
TSUNAMI_EXE_MDB=$(BUILD_DIR)/Tsunami.exe.mdb
MONOTORRENT_DLL_SOURCE=../libs/MonoTorrent.dll

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:TRACE"
ASSEMBLY = bin/Release/Tsunami.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release/

TSUNAMI_EXE_MDB=
MONOTORRENT_DLL_SOURCE=../libs/MonoTorrent.dll

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=Tsunami.resources.dll

PROGRAMFILES = \
	$(TSUNAMI_EXE_MDB) \
	$(MONOTORRENT_DLL)  

BINARIES = \
	$(TSUNAMI)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES) 

FILES = \
	CommandContexts/DirectoryManagement/AddDirectoryContext.cs \
	CommandContexts/DirectoryManagement/DirectoryContext.cs \
	CommandContexts/ListenerManagement/AddListenerContext.cs \
	CommandContexts/Context.cs \
	CommandContexts/GeneralContext.cs \
	CommandContexts/DirectoryManagement/ListDirectoriesContext.cs \
	CommandContexts/ListenerManagement/ListenerContext.cs \
	CommandContexts/ListenerManagement/ListListenersContext.cs \
	CommandContexts/ListenerManagement/RemoveListenerContext.cs \
	CommandContexts/Options/Option.cs \
	CommandContexts/Options/Options.cs \
	CommandContexts/DirectoryManagement/RemoveDirectoryContext.cs \
	CommandContexts/StatisticsContext.cs \
	getline.cs \
	MainClass.cs \
	TrackerHost.cs \
	AssemblyInfo.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	tsunami.in 

REFERENCES =  \
	System \
	System.Data \
	System.Xml

DLL_REFERENCES =  \
	../libs/MonoTorrent.dll

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

include $(top_srcdir)/Makefile.include

TSUNAMI = $(BUILD_DIR)/tsunami
MONOTORRENT_DLL = $(BUILD_DIR)/MonoTorrent.dll

$(eval $(call emit-deploy-wrapper,TSUNAMI,tsunami,x))
$(eval $(call emit-deploy-target,MONOTORRENT_DLL))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
